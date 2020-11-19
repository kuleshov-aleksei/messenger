# Настройка Rabbit MQ

```
docker create  \
	--name=rabbitmq \
	--hostname rpi-mq-01 \
	-e PUID=1000 \
	-e PGID=1000 \
	-e TZ=Europe/Moscow \
	-p 5672:5672 \
	-v /home/messenger/rabbitmq/rabbitmq.config:/etc/rabbitmq/rabbitmq.config:ro \
	--restart unless-stopped \
	rabbitmq:3

	
docker start rabbitmq
```

Плагин для управления:

```
docker create  \
	--name=rabbitmq-management \
	--hostname rpi-mq-01 \
	-e PUID=1000 \
	-e PGID=1000 \
	-e TZ=Europe/Moscow \
	-p 8083:15672 \
	--restart unless-stopped \
	rabbitmq:3-management
	
docker start rabbitmq-management
```

Админка находится на http://ip:8083