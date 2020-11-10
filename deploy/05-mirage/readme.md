# Настройка mirage

https://github.com/appbaseio/mirage

```
docker create \
  --name=mirage \
  -e PUID=1000 \
  -e PGID=1000 \
  -e TZ=Europe/Moscow \
  -p 3030:3030 \
  --restart unless-stopped \
  appbaseio/mirage

docker start mirage
```

Далее нужно разрешить CORS в эластике:

```bash
docker exec -t -i elasticsearch /bin/bash
vi config/elasticsearch.yml
```

```
http.cors.allow-origin: "/.*/"
http.cors.enabled: true
http.cors.allow-headers: X-Requested-With,X-Auth-Token,Content-Type, Content-Length, Authorization
http.cors.allow-credentials: true
```
