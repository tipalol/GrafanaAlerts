# GrafanaAlerts
Web api to hook Grafana alerts and create trouble tickets from them

## Use
1. Build and run Docker image

```
docker build -t aspnetapp .  

docker run -d -p 8080:80 --name GrafanaAlerts aspnetapp
```

2. Create new WebHook notification channel in Grafana with url - "yourAppUrl/api/trigger"
3. Every alert in this channel will trigger dotnet method TriggerAlert which does parsing, complementing and registering of new trouble ticket in the system
