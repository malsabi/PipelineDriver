# PipelineDriver
[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

## About
Interprocess communication provides pipelining for process communication.

In this project we are using named pipeline, such that it can be private pipeline only for two specific processes.

The project is using [IViewNET](https://github.com/malsbi/IViewNet) library which provides useful methods for IPC.

There are two classes:

1. IViewPipeServer: Responsible for waiting any new connection, once a connection is made, both end points
   can communicate and send structured data.

2. IviewPipeClient: Responsible for connecting to a specific named pipe server, once a connection is 
   successfully established, both end points can communicate and send structured data.
