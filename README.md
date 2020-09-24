# RandomCoffee
A simple Azure Function that takes a list of names and returns those same names as randomized paired matches


Random Coffee is the simple idea of matching two random people for coffee and a chat.  It can be used to helps groups of people get to know one another.


This Azure Function simply performs the act of randomizing those matches.  How those group members sign up to the initative and how these matches are communicated to them is left to you to implement.  As an example I personally use this Azure Function to connect to a Yammer Group on a mounthly basis, read the membership of that group and then post the randomized matches back to that group.

The Azure Function is totally random every time it is called, it does not record any previous matches made and does not record any information passed to it.
