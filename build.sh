#!/bin/bash

set -e

version=${1:?Input version}
outDir=$(realpath pack)

if [[ "$2" == "--push" ]]; then
    source=${3:?--push <source> <api key>}
    apiKey=${4:?--push <source> <api key>}
fi

function normalize_os {
  case "$1" in
    "darwin")
      echo "osx"
      ;;
    "windows")
      echo "win"
      ;;
    "linux")
      echo "$1"
      ;;
    *)
      echo "Unknown os '$1'" >&2
      exit 1
      ;;
  esac
}

function normalize_architecture {
  case "$1" in
    "aarch64")
      echo "arm64"
      ;;
    "x86_64")
      echo "x64"
      ;;
    "x86"|"x64"|"arm64")
      echo "$1"
      ;;
    *)
      echo "Unknown architecture '$1'" >&2
      exit 1
      ;;
  esac
}

function to_lower {
   echo $1 | tr '[:upper:]' '[:lower:]'
}

function openjre_get_release_value {
    local release="$1/obj/openjre/release"
    local value=$(grep "$2=" $release | cut -d'=' -f2)
    echo $value | perl -pe 's/["\r\n\t\f\v]//g'
}

function openjre_verify {
    local path=$1
    local csproj="$path/$(basename $runtime).csproj"

    echo "[openjre] verify runtime identifier"

    local name=$(to_lower "$(openjre_get_release_value $path OS_NAME)")
    local arch=$(to_lower "$(openjre_get_release_value $path OS_ARCH)")

    local normalizedName=$(normalize_os $name)
    local normalizedArch=$(normalize_architecture $arch)

    if [[ "$normalizedName" == "linux" ]]; then
        local libc=$(to_lower "$(openjre_get_release_value $path LIBC)")
        if [[ "$libc" == "musl" ]]; then
            normalizedName="linux-musl"
        fi
    fi

    echo "[openjre] platform: $normalizedName ($name)"
    echo "[openjre] architecture: $normalizedArch ($arch)"

    local actualRID="$normalizedName-$normalizedArch"
    local expectedRID=$(awk -v FS="(>|<)" '/<RuntimeIdentifier>/ {print $3}' $csproj)

    if [[ "$expectedRID" != "$actualRID" ]]; then
        echo "Expected runtime identifier is '$expectedRID' but was '$actualRID'" >&2
        exit 1
    fi
}

function dotnet_restore {
    local path=$1

    pushd $path > /dev/null
    echo "[dotnet] restore"
    dotnet restore --no-cache
    popd > /dev/null
}

function dotnet_pack {
    local path=$1

    pushd $path > /dev/null
    echo "[dotnet] pack"
    dotnet pack -c Release -o $outDir -p:Version=$version --nologo --no-restore ${@:2}
    popd > /dev/null
}

function dotnet_nuget_push {
    local nupkg=$1
    local source=$2
    local apiKey=$3

    echo "[dotnet] $nupkg push to $source"
    dotnet nuget push $nupkg --source $source --api-key $apiKey
}

function nupkg_remove_lib {
    local project="$(basename $1)"
    local nupkg="$project.$version.nupkg"
    local tmpDir="$nupkg.tmp"
    local nuspec="$tmpDir/$project.nuspec"

    pushd $outDir > /dev/null

    echo "[nupkg] patch $nupkg"

    # Unzip
    echo "[nupkg] unpack"
    unzip $nupkg -d $tmpDir 1> /dev/null

    # Patch .nuspec
    echo "[nupkg] remove 'dependencies' tag"

    if [[ "$OSTYPE" == "darwin"* ]]; then
        sed -i '' '/<dependencies>/,/<\/dependencies>/d' "$nuspec"
    else
        sed -i '/<dependencies>/,/<\/dependencies>/d' "$nuspec"
    fi

    # Remove lib
    echo "[nupkg] remove 'lib/*' dir"
    rm -rf $tmpDir/lib

    # Zip
    rm $nupkg
    cd $tmpDir
    zip -r ../$nupkg . -x '**/__MACOSX' -x '**/.DS_Store' 1> /dev/null
    rm -rf ../$tmpDir

    echo "[nupkg] repack"

    popd > /dev/null
}

rm -rf $outDir

for runtime in src/runtimes/J2NET.Runtime.* ; do
    echo " ** $runtime ** "
    dotnet_restore $runtime
    openjre_verify $runtime
    dotnet_pack $runtime -p:JREVersion="$(openjre_get_release_value $runtime IMPLEMENTOR_VERSION)"
    nupkg_remove_lib $runtime
    
    echo
done

echo " ** J2NET ** "
dotnet_restore src/J2NET
dotnet_pack src/J2NET

if [[ "$2" == "--push" ]]; then
    for nupkg in $outDir/*.nupkg ; do
        dotnet_nuget_push $nupkg $source $apiKey
    done
fi
