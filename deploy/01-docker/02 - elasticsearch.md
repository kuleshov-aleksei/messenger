# Create docker container for elasticsearch


```bash
docker create \
  --name=elasticsearch \
  -e PUID=1000 \
  -e PGID=1000 \
  -e TZ=Europe/Moscow \
  -e "discovery.type=single-node" \
  -e ES_JAVA_OPTS="-Xms200m -Xmx2000m" \
  -p 9200:9200 \
  -p 9300:9300 \
  --restart unless-stopped \
  elasticsearch:7.9.2
  
docker start elasticsearch
```

### Create cerebro container

```bash
docker create \
  --name=cerebro \
  -p 9000:9000 \
  --restart unless-stopped \
  lmenezes/cerebro
  
docker start cerebro
```

