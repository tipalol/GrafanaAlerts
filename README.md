# GrafanaAlerts
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

4. Settings located inside "appsettings.json" such as: TicketSystemHost, SoapTemplateRequestsLocation, JaegerAgentHost and etc.

5. Every alert in this channel will trigger dotnet method TriggerAlert which does parsing, complementing and registering of new trouble ticket in the system
