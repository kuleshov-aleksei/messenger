# Настройка nginx

```bash
docker create \
  --name=nginx-api \
  -e PUID=1000 \
  -e PGID=1000 \
  -e TZ=Europe/Moscow \
  -p 80:80\
  -v/home/messenger/nginx/nginx.conf:/etc/nginx/nginx.conf:ro \
  --restart unless-stopped \
  arm64v8/nginx

docker start nginx-api
```

