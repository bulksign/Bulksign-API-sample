﻿using System;
using System.IO;
using Bulksign.Api;
using Bulksign_Api_Samples;

namespace Bulksign.ApiSamples.Scenarios
{
	public class SetFormFieldValues
	{
		public void SendBundle()
		{

			BulkSignApi api = new BulkSignApi();

			BulksignBundle bb = new BulksignBundle();
			bb.DaysUntilExpire = 10;
			bb.DisableNotifications = false;
			bb.NotificationOptions = new BulksignNotificationOptions();
			bb.Message = "Please sign this document";
			bb.Subject = "Please Bulksign this document";
			bb.Name = "Test bundle";

			BulksignRecipient firstRecipient = new BulksignRecipient();
			firstRecipient.Name = "Bulksign Test";
			firstRecipient.Email = "contact@bulksign.com";
			firstRecipient.Index = 1;
			firstRecipient.RecipientType = BulksignApiRecipientType.Signer;

			bb.Recipients = new[]
			{
					 firstRecipient
			};

			BulksignDocument document = new BulksignDocument();
			document.Index = 2;
			document.FileName = "forms.pdf";
			document.ContentBytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\forms.pdf");

			//set pdf from fields values

			document.SetFormFieldValues = new BulksignSetFormFieldValue[2];
			document.SetFormFieldValues[0] = new BulksignSetFormFieldValue()
			{
				FieldName = "Text1",
				FieldValue = "This is a test text"
			};

			document.SetFormFieldValues[1] = new BulksignSetFormFieldValue()
			{
				FieldName = "Group3",
				FieldValue = "Choice2"
			};

			bb.Documents = new[]
			{
				document
			};

			
			BulksignAuthorization token = new ApiKeys().GetAuthorizationToken();

			if (string.IsNullOrEmpty(token.UserToken))
			{
				Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
				return;
			}


			BulksignResult<BulksignSendBundleResult> result = api.SendBundle(token, bb);

			Console.WriteLine("Api call is successfull: " + result.IsSuccessful);

			if (result.IsSuccessful)
			{
				Console.WriteLine("Access code for recipient " + result.Response.AccessCodes[0].RecipientName + " is " + result.Response.AccessCodes[0].AccessCode);
				Console.WriteLine("Bundle id is : " + result.Response.BundleId);
			}

		}
	}
}