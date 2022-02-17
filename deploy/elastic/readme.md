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

### Copy es dictionaries into container:

```bash 
docker exec -it es01 /bin/bash
mkdir -p config/analysis/ru_RU
mkdir -p config/hunspell/ru_RU
exit
docker cp stopwords-ru.txt es01:/usr/share/elasticsearch/config/analysis/ru_RU/
docker cp ru_RU.aff es01:/usr/share/elasticsearch/config/hunspell/ru_RU
docker cp ru_RU.dic es01:/usr/share/elasticsearch/config/hunspell/ru_RU
```

### Create mapping

You can transfer data using [elasticdump](https://stackoverflow.com/questions/26547560/how-to-move-elasticsearch-data-from-one-server-to-another):

```
npm install -g elasticdump
elasticdump --input=http://mysrc.com:9200/my_index --output=http://mydest.com:9200/my_index --type=mapping
elasticdump --input=http://mysrc.com:9200/my_index --output=http://mydest.com:9200/my_index --type=data
```