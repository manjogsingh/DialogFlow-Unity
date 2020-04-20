﻿using System;
using System.Collections.Generic;
namespace JsonData
{
	// Request body
	[Serializable]
	public class RequestBody
	{
		public QueryInput queryInput;
	}

	[Serializable]
	public class QueryInput
	{
		public TextInput text;
	}

	[Serializable]
	public enum WebhookState
	{
		STATE_UNSPECIFIED,
		WEBHOOK_STATE_ENABLED,
		WEBHOOK_STATE_ENABLED_FOR_SLOT_FILLING
	}

	[Serializable]
	public class TextInput
	{
		public String text;
		public String languageCode;
	}

	// Response body

	[Serializable]
	public class ResponseBody
	{
		public string responseId;
		public QueryResult queryResult;
		public Status webhookStatus;
	}

	[Serializable]
	public class QueryResult
	{
		public string queryText;
		public string languageCode;
		public int speechRecognitionConfidence;
		public string action;
		public Struct parameters;
		public bool allRequiredParamsPresent;
		public string fulfillmentText;
		public Message[] fulfillmentMessages;
		public string webhookSource;
		public Struct webhookPayload;
		public Context[] outputContexts;
		public Intent intent;
		public int intentDetectionConfidence;
		public Struct diagnosticInfo;
	}

	[Serializable]
	public class Status
	{
		public int code;
		public string message;
		public Object[] details;
	}

	[Serializable]
	public class Intent
	{
		public string name;
		public string displayName;
		public WebhookState webhookState;
		public int priority;
		public bool isFallback;
	}

	[Serializable]
	public class Context
	{
		public string name;
	}

	[Serializable]
	public class Struct
	{
		public Dictionary<string, Value> fields;
	}

	[Serializable]
	public class Value
	{
		public NullValue null_value;
		public double number_value;
		public string string_value;
		public bool bool_value;
		public Struct struct_value;
		public ListValue list_value;

		public void ForBool(bool value)
		{
			this.bool_value = value;
		}

		public void ForString(string value)
		{
			this.string_value = value;
		}

		public void ForNumber(double number)
		{
			this.number_value = number;
		}

		public void ForNull()
		{
			this.null_value = NullValue.null_vaule;
		}

		public void ForStruct(Struct value)
		{
			this.struct_value = value;
		}

		public void ForList(ListValue value)
		{
			this.list_value = value;
		}
	}

	[Serializable]
	public enum NullValue
	{
		null_vaule
	}

	[Serializable]
	public class ListValue
	{
		public Value values;

	}

	[Serializable]
	public class Text
	{
		public string[] text;
	}

	[Serializable]
	public class Message
	{
		public Text text;
	}
}