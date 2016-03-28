# ProcessBus
This is a simple abstraction layer over LanguageExt.Process to wrangle servicebus functionality out of it. It started as an excuse to see how writing Haskell in C# felt, using LanguageExt.

## Usage

### Configuration
We start with a routing configuration. Because we define our routing topology in YAML, it is quite easy to do some quite complex wiring up of various components of your architecture. 

```Yaml
Transports:

# Events
- Path: UserJoined
  ForwardTo:
  - Email
- Path: Purchase
  ForwardTo:
  - Email
- Path: PrivateMessage
  ForwardTo:
  - WebSockets

# Endpoints
- Path: Email
- Path: WebSockets

# Logging
- Path: Logging
  ForwardFrom:
  - UserJoined
  - Purchase
  - PrivateMessage
```

Let's say a user joins your site. All your backend has to do is raise an event, `UserJoined`. This configuration will have the `UserJoined` message automatically forwarded to your email endpoint, where you could handle it to send a welcome email, for example. We can decouple our services very easily if our event raisers don't have to know where the messages should be delivered.

Similarly, the `PrivateMessage` is set up to automatically forward to the `WebSockets` endpoint, but if we wanted to send emails on private messages as well, we would only have to add a forward to the `Email` endpoint:

````Yaml
- Path: PrivateMessage
  ForwardTo:
  - WebSockets
  - Email
 ```

Note that the logging endpoint is configured to receive *from* events, so we don't have to clutter up every event with a forward to the logging endpoint.

### Flexibility
Let's say we want to add a monitoring dashboard. It doesn't take long to modify the configuration to handle this in an elegant way:

```Yaml
Transports:

# Events
- Path: UserJoined
  ForwardTo:
  - Email
- Path: Purchase
  ForwardTo:
  - Email
- Path: PrivateMessage
  ForwardTo:
  - WebSockets

 # Forward all events to this transport
- Path: Events 
  ForwardFrom:
  - UserJoined
  - Purchase
  - PrivateMessage

# Endpoints
- Path: Email
- Path: WebSockets

# Logging
- Path: Logging
  ForwardFrom:
  - Events

# Monitoring
- Path: Monitor
  ForwardFrom:
  - Events
```

We just add a new `Events` path that grabs all the events and groups them under one name. This means that instead of listing out every event for the `Logging` and `Monitor` endpoints, we can just get them to listen to the grouped `Event` endpoint.

### Validation
There's quite a bit of validation on the configuration, for example:

#### Typos
```Yaml
Transports:
- Path: EventA
- Path: EventB
  ForwardTo:
  - EvetnC
- Path: EventC  
```

gives:

```> Configuration specifies forward to non-existent transport "EvetnC".```

#### Cyclic forwarding 

```Yaml
Transports:
- Path: EventA
  ForwardTo:
  - EventB
- Path: EventB
  ForwardTo:
  - EventC
- Path: EventC  
  ForwardTo:
  - EventA
```
gives:

```> Messaging topologies with cycles are not supported.```

### Messages
It's recommended that you create a class library that just contains your messages, so they can be referenced by every project. Messages are just POCOs:

```C#
public class UserJoinedMessage 
{
	public User User { get; }
	public DateTime JoinedDate { get; }
}
```

### Sending
If you're using C#6, you can just import the `SendMessage` class:

```C#
using static ProcessBus.SendMessage;
```

Let's say the user calls an API method to join. You can just add the following line:

```C#
public async Task<Response> Join(UserInfo userInfo) 
{
	var userJoinedResult = await joinUser(userInfo);
	if (userJoinedResult.Success) Send("UserJoined", user);
}
```

If you're not using C#6, you'll have to call `SendMessage.Send`.

### Receiving
Receiving messages is quite simple:

```C#
using static ProcessBus.Subscriber;

Subscribe("Monitor")
	.Handle<UserJoinedMessage(m => {
		Console.WriteLine($"User {m.User.Name} joined.")
	});
```

### Router
Of course, none of this will work if you don't have a router controlling where all the messages are sent.

```C#
using static ProcessBus.RouterFunctions;
using static ProcessBus.YamlParser;

var config = ParseFromFile("config.yaml");
SpawnRouterFromConfig(config);
```
