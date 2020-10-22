# Настройка envoy

docker create  \
	--name=envoy \
	-e PUID=1000 \
	-e PGID=1000 \
	-e TZ=Europe/Moscow \
	-p 10000:10000 \
	-v/home/messenger/envoy/envoy.yaml:/etc/envoy/envoy.yaml:ro \
	--restart unless-stopped \
	envoyproxy/envoy-dev
	
docker start envoy