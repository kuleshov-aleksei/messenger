# Настройка Rabbit MQ

Ставим rabbit mq с установленным плагином администрирования

```
docker create  \
	--name=rabbitmq \
	--hostname rpi-mq-01 \
	-e PUID=1000 \
	-e PGID=1000 \
	-e TZ=Europe/Moscow \
	-p 8083:15672 \
	-v /home/messenger/rabbitmq/rabbitmq.conf:/etc/rabbitmq/rabbitmq.conf \
	--restart unless-stopped \
	rabbitmq:3-management
	
docker start rabbitmq
```

Админка находится на http://ip:8083

