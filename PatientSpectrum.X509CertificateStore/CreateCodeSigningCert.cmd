makecert -r -pe -n "CN=idsrvtokencert" -b 01/01/2017 -e 01/01/2037 -sv idsrvtokencert.pvk -eku 1.3.6.1.5.5.7.3.3 -sky signature -a sha256 -len 2048 -ss my -sr LocalMachine idsrvtokencert.cer
pvk2pfx.exe -pvk idsrvtokencert.pvk -spc idsrvtokencert.cer -pfx idsrvtokencert.pfx