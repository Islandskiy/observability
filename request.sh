#!/bin/zsh

i=0
while [ $i -lt 10000 ]
do
	echo "$i:"
	
	curl -X GET $1
	echo

#	ms=$[ ($RANDOM % 1000 + 1) / 1000.0 ]
	ms=$[ ($RANDOM % 100 + 1) / 1000.0 ]
	echo $ms
	sleep $ms
	(( i++ ))
done
