# Messenger

# Deployment:

1. Create network

```
docker network create messenger_network
```

2. Deploy environment

```
docker-compose --env-file .\.env.environment -f .\docker-compose.environment.yml up -d
```

3. Deploy services

```
docker-compose --env-file .\.env.services -f .\docker-compose.yml up -d
```

