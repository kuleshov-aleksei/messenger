# Create docker container for mysql

```bash
docker create \
  --name=mysql-messenger \
  -e PUID=1000 \
  -e PGID=1000 \
  -e TZ=Europe/Moscow \
  -e MYSQL_ROOT_PASSWORD=EqxM9tuOOsocoj \
  -p 3306:3306\
  --restart unless-stopped \
  mysql/mysql-server:latest
  
docker start mysql-messenger
```

Создаем пользователя dcsm
```sql
docker exec -it mysql-messenger mysql -uroot -p
# enter password EqxM9tuOOsocoj 
CREATE USER 'dcsm'@'%' IDENTIFIED WITH mysql_native_password BY 'dcsm';
GRANT ALL PRIVILEGES ON *.* TO 'dcsm'@'%' WITH GRANT OPTION;
exit;
```

Создаем базу данных

```sql
docker exec -it mysql-messenger mysql -uroot -p
# enter password EqxM9tuOOsocoj 
CREATE DATABASE dcsm;
exit;
```

