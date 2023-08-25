# HyperSoa
.NET implementation of the Hyper back-end service libraries featuring a platform-independent, language-agnostic communication framework.

- Supports DI, including the options pattern for hosting options
- Supports interop with the old WCF-based version in the .NET Framework
- Supports ILogger interface at two levels
  - Service level (verbose trace-level logs)
  - Command level (command-specific logging)

### Custom Command Modules
Create a class that implements ICommandModule and add it to the service configuration to be called remotely by a unique name. Implement IAwaitableCommandModule instead if you require async/await semantics.

### Custom Activity Monitors
Each command emits activity events that can be monitored by user-defined code for observability. This is intended as a high-level way to report progress or internal workings of the command; this is separate from the lower-level logging functionality provided by the  ILogger interface.

### Remote Administration
Some aspects of the service can be administered remotely, including remote cancellation of long-running tasks and enabling/disabling various features such as progress caching, the max concurrent task limit, and diagnostics.