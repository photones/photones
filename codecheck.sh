#!/bin/bash

exitcode=0

root=`git rev-parse --show-toplevel`
echo "Root of repository: $root"
echo

cd "$root"

# Check if source files contain tab characters.
files_with_tabs=$(git grep -Pla '\t' -- *.fsx *.fs *.cs)
if [[ -n "$files_with_tabs" ]]; then
    echo
    echo "The following files contain tab characters:"
    echo
    echo "$files_with_tabs"
    exitcode=1
fi

# To replace all tabs: (warning! this introduces lf endings)
#echo "$files_with_tabs" | xargs sed -zi 's/\t/    /g'

# Check if all lines in source files end with a unix line ending
files_with_crlf=$(git ls-files *.fsx *.fs *.cs | xargs grep -UPlza '\r\n')
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
too_long_lines=$(git grep -Pn '(?<=.{100}).+' -- *.fsx *.fs *.cs)
if [[ -n "$too_long_lines" ]]; then
    echo
    echo "The following lines are too long:"
    echo
    # Repeat command for colored output
    git grep -Pn '(?<=.{100}).+' -- *.fsx *.fs *.cs
    exitcode=1
fi

echo
[[ $exitcode -eq 0 ]] && echo Passed
[[ $exitcode -ne 0 ]] && echo Failed
exit $exitcode
