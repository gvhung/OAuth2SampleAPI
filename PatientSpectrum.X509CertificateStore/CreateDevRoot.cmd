makecert.exe -r -n "CN=DevRoot" -pe -sv DevRoot.pvk -a sha1 -len 2048 -b 01/21/2017 -e 01/21/2037 -cy authority DevRoot.cer
pvk2pfx.exe -pvk DevRoot.pvk -spc DevRoot.cer -pfx DevRoot.pfx