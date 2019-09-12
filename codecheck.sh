#!/bin/bash

exitcode=0

root=`git rev-parse --show-toplevel`
echo "Root of repository: $root"
echo

cd "$root"
source_files=$(find . \( -name \*.cs -or -name \*.fs \) \
    \( -path "./Bearded/*" \
    -or -path "./Bearded.Test/*" \
    -or -path "./GameLogic/*" \)\
    -not -path "./obj/*" \
    -not -path "./bin/*")


# Check if source files contain tab characters.
files_with_tabs=$(echo "$source_files" | xargs grep -UPla '\t')
if [[ -n "$files_with_tabs" ]]; then
    echo
    echo "The following files contain tab characters:"
    echo
    echo "$files_with_tabs"
    exitcode=1
fi

# To replace all tabs: (warning! this introduces lf endings)
#echo "$files_with_tabs" | xargs sed -zi 's/\t/    /g'

# Check if all lines in source files end with a windows line ending
files_with_crlf=$(echo "$source_files" | xargs grep -UPlza '\r\n')
if [[ -n "$files_with_crlf" ]]; then
    echo
    echo "The following files contain windows line endings:"
    echo
    echo "$files_with_crlf"
    exitcode=1
fi

# To replace all lf with crlf:
#echo "$files_with_crlf" | xargs dos2unix

# Check if the length of all source lines are limited by some predefined constant.
# Both for readability, and to catch respective zos compilation errors early on.
files_with_too_long_lines=$(echo "$source_files" | xargs grep -la '.\{100\}')
if [[ -n "$files_with_too_long_lines" ]]; then
    echo
    echo "The following files contain lines that are too long:"
    echo
    echo "$files_with_too_long_lines"
    exitcode=1
fi

echo
[[ $exitcode -eq 0 ]] && echo Passed
[[ $exitcode -ne 0 ]] && echo Failed
exit $exitcode
