#!/bin/bash
# Usage:
# ./createcertfilesfrompfx.sh /path/to/domain.pfx
#
# Creates domain.pem and domain.key in the current directory
# Based on "Howto convert a PFX to a seperate .key & .crt file" https://gist.github.com/ericharth/8334664#gistcomment-1942267
pfxpath="C:\Users\leone\Documents\GITHUB repositories\app-inventario-xamario 2.0 - desktop\Inventario2.UWP"

if [ ! -f "$pfxpath" ];
then
  echo "Cannot find PFX using path '$pfxpath'"
  exit 1
fi

crtname=`basename ${pfxpath%.*}`
domaincacrtpath=`mktemp`
domaincrtpath=`mktemp`
fullcrtpath=`mktemp`
keypath=`mktemp`
passfilepath=`mktemp`
read -s -p "PFX password: " pfxpass
echo -n $pfxpass > $passfilepath

echo "Creating .CRT file"
openssl pkcs12 -in $pfxpath -out $domaincacrtpath -nodes -nokeys -cacerts -passin file:$passfilepath
openssl pkcs12 -in $pfxpath -out $domaincrtpath -nokeys -clcerts -passin file:$passfilepath
cat $domaincrtpath $domaincacrtpath > $fullcrtpath
rm $domaincrtpath $domaincacrtpath

echo "Creating .KEY file"
openssl pkcs12 -in $pfxpath -nocerts -passin file:$passfilepath -passout pass:Password123 \
| openssl rsa -out $keypath -passin pass:Password123

rm $passfilepath

mv $fullcrtpath ./${crtname}.pem
mv $keypath ./${crtname}.key

ls -l ${crtname}.pem ${crtname}.key