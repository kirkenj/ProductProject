﻿cat /usr/share/nginx/html/appsettings.json | jq --arg aVar "$(printenv URL_BACKEND)" '.URL_BACKEND = $aVar' > /usr/share/nginx/html/appsettings.json
