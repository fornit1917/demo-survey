docker run \
 -e ASPNETCORE_URLS='http://localhost:8920' \
 -e ASPNETCORE_ENVIRONMENT=Production \
 --network host \
 --restart always \
 --name demo-survey \
 -d \
 fornit1917/demo-survey:latest