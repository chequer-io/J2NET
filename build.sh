#!/bin/sh

set -e

version=${1:?Input version}
outDir=$(realpath pack)

function dotnet_pack() {
    local path=$1

    pushd $path > /dev/null
    dotnet pack -c Release -o $outDir -p:Version=$version
    popd > /dev/null
}

function nupkg_remove_lib() {
    local project=$1
    local nupkg="$project.$version.nupkg"
    local tmpDir="$nupkg.tmp"
    local nuspec="$tmpDir/$project.nuspec"

    pushd $outDir > /dev/null

    # Unzip
    unzip $nupkg -d $tmpDir

    # Patch .nuspec
    if [[ "$OSTYPE" == "darwin"* ]]; then
        sed -i '' '/<dependencies>/,/<\/dependencies>/d' "$nuspec"
    else
        sed -i '/<dependencies>/,/<\/dependencies>/d' "$nuspec"
    fi

    # Remove lib
    rm -rf $tmpDir/lib

    # Zip
    rm $nupkg
    cd $tmpDir
    zip -r ../$nupkg . -x '**/__MACOSX' -x '**/.DS_Store'
    rm -rf ../$tmpDir

    popd > /dev/null
}

rm -rf $outDir

for runtime in Runtimes/J2NET.Runtime.* ; do
    dotnet_pack $runtime
    nupkg_remove_lib "$(basename $runtime)"
done

dotnet_pack Core/J2NET
