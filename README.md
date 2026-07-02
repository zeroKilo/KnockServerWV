# KnockServerWV

Port-knocking server and client for windows

<img width="802" height="482" alt="image" src="https://github.com/user-attachments/assets/cc109ba4-c00f-4d3e-a3a7-757b7e80a494" />

**Notice: Sofar only IPv4 is supported!**


<h2>How to setup Server</h2>

1. Edit config file for server
```
maxlogdisplay=1000 <-max displayed log lines in form, log.txt is not affected by this
usertimeout=60 <-timeout per authorized user in second, can be reset by running client again
baseport=7000 <-baseport number for knocking ports
range=10 <-how many ports are used (here its 7000-7009)
length=10 <-length of knocking sequence
salt1=12 <-salts for randomizing daily knocking sequence
salt2=34
```

The server will then bind to 0.0.0.0 and these ports to listen for knocks

2. Add inbound firewall rule for knockserver

<img width="433" height="581" alt="image" src="https://github.com/user-attachments/assets/ac4c9416-e6c4-45a1-88a1-21ebda41172d" />
<img width="433" height="581" alt="image" src="https://github.com/user-attachments/assets/b2ff4761-733e-4361-805f-f03393fea554" />
<img width="433" height="581" alt="image" src="https://github.com/user-attachments/assets/50cd476b-2374-43a2-b3f5-224166127b98" />

the server will control the field "Remote IP address" later, here initially only 127.0.0.1 is allowed so its technically open but practically closed

4. Add the rule name to rules.txt
   
5. Edit properties of KnockServerWV.exe to run as administrator (needed for firewall access)
   
<img width="363" height="525" alt="image" src="https://github.com/user-attachments/assets/9bc0ae28-d43d-4251-a14d-d4477f40707e" />

6. Run it, the log should say that the rule was added and updated

<img width="802" height="482" alt="image" src="https://github.com/user-attachments/assets/733ce819-fefc-4341-96a9-718550c553b2" />

<h2>How to setup Client</h2>

1. Edit config file for client

```
target=XXX.XXX.XXX.XXX <-IP of knockserver
baseport=7000 <-baseport for knocks, must match server side
range=10 <-range of ports, must match server side
length=10 <-length of knock sequence, must match server side
salt1=12 <-salts for generating sequence, must match server side
salt2=34
```

2. Run client twice to verify authentication

<img width="979" height="512" alt="image" src="https://github.com/user-attachments/assets/1aa9755d-279e-4b17-bb59-4760dab51a51" />

On server side you should see

<img width="802" height="482" alt="image" src="https://github.com/user-attachments/assets/4a4484b1-bb95-4cd8-8ab8-51774d8a9d77" />

<h2>Controlling other firewall rules with knock server</h2>

Just add their name to the rules.txt list and restart the server, it should pick it up and update the allowed IPs on the fly
