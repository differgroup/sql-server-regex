# Match

![SQL Regex Logo](/images/sql-regex-logo.png)

RegexMatch() is a scalar function that lets you run a regular expression against a string. It is a SQL CLR function that exposes the [System.Text.RegularExpressions](https://msdn.microsoft.com/en-us/library/system.text.regularexpressions(v=vs.110).aspx)' [Match()](https://msdn.microsoft.com/en-us/library/b9712a7w(v=vs.110).aspx) method.

Let's look at a few examples, inspired by a handy [Regular Expressions tutorial](http://www.regular-expressions.info/examples.html)


### Screenshot

![Match SSMS example](/images/match-1.png)


### IP Addresses

Let's pull an IPv4 address out of a string. 

```
declare @regex_pattern varchar(max) = '\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b'

select dbo.RegexMatch('10.1.1.1', @regex_pattern)

select dbo.RegexMatch('The IP address detected was 10.1.1.1 at 10:30pm', @regex_pattern)
```

Note: this simple example will match some non-valid IP addresses as well (e.g. 999.999.999.999). You can make a more exact regular expression using a more complicated pattern, such as:

'\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.
  (25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.
  (25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.
  (25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b'


### Floating Point Numbers

Let's pull out a floating-point number.

```
declare @regex_pattern varchar(max) = '[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?'

select dbo.RegexMatch('Avogrado''s number is 6.0221409e+23', @regex_pattern)
```
