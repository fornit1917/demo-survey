TAG=$1
docker build -t fornit1917/demo-survey:$TAG .
docker push fornit1917/demo-survey:$TAG