﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
//using Serilog;

namespace Mozu.Api.ToolKit.Logging
{
	public static class LoggingService
	{
		public static ILoggerFactory LoggerFactory { get; set; }// = new LoggerFactory();
		public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
		public static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);

		public static ILogger LoggerFor<T>() => CreateLogger<T>();
		public static ILogger LoggerFor(Type type) => CreateLogger(type.FullName);
		public static ILogger LoggerFor(string name) => CreateLogger(name);
	}
}
