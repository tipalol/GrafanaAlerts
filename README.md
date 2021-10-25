# GrafanaAlerts

[![.NET](https://github.com/tipalol/GrafanaAlerts/actions/workflows/dotnet.yml/badge.svg)](https://github.com/tipalol/GrafanaAlerts/actions/workflows/dotnet.yml)

Web api to hook Grafana alerts and create trouble tickets from them

## Use
1. Build and run Docker image

```
docker build -t aspnetapp .  

docker run -d -p 8080:80 --name GrafanaAlerts aspnetapp
```

2. Create new WebHook notification channel in Grafana with url - "dockerContainerUrl/api/trigger"

3. What information must alert contain: 

* Ticket name - from Alert name
* Ticket description - from Alert message
* Ticket role - from Alert tag "role"
* Ticket priority - from Alert tag "priority"
* Ticket ke - from Alert tag "ke"
* Ticket initiatorType - from Alert tag "initiatorType"
* Ticket initatorRole - from Alert tag "iniatorRole"

4. Settings located inside "AppData/Configs/config.json" such as: TicketSystemHost, SoapTemplateRequestsLocation and etc.

5. Every alert in this channel will trigger method TriggerAlert which does parsing, complementing and registering of new trouble ticket in the system
