﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif.Cli.Rules
{
    public class MessagesShouldEndWithPeriod : SarifValidationSkimmerBase
    {
        public override string FullDescription => RuleResources.SV0008_MessagesShouldEndWithPeriod;

        public override ResultLevel DefaultLevel => ResultLevel.Warning;

        /// <summary>
        /// SV0008
        /// </summary>
        public override string Id => RuleId.MessagesShouldEndWithPeriod;

        protected override IEnumerable<string> FormatIds
        {
            get
            {
                return new string[]
                {
                    nameof(RuleResources.SV0008_Default)
                };
            }
        }

        protected override void Analyze(AnnotatedCodeLocation annotatedCodeLocation, string annotatedCodeLocationPointer)
        {
            Analyze(annotatedCodeLocation.Message, annotatedCodeLocationPointer);
        }

        protected override void Analyze(CodeFlow codeFlow, string codeFlowPointer)
        {
            Analyze(codeFlow.Message, codeFlowPointer);
        }

        protected override void Analyze(Notification notification, string notificationPointer)
        {
            Analyze(notification.Message, notificationPointer);
        }

        protected override void Analyze(Result result, string resultPointer)
        {
            Analyze(result.Message, resultPointer);
        }

        protected override void Analyze(Rule rule, string rulePointer)
        {
            if (rule.MessageFormats != null)
            {
                foreach (string formatId in rule.MessageFormats.Keys)
                {
                    string messageFormat = rule.MessageFormats[formatId];
                    if (DoesNotEndWithPeriod(messageFormat))
                    {
                        string messagePointer = rulePointer
                            .AtProperty(SarifPropertyName.MessageFormats)
                            .AtProperty(formatId);

                        LogResult(
                            messagePointer,
                            nameof(RuleResources.SV0008_Default),
                            messageFormat);
                    }
                }
            }
        }

        protected override void Analyze(Stack stack, string stackPointer)
        {
            Analyze(stack.Message, stackPointer);
        }

        protected override void Analyze(StackFrame frame, string framePointer)
        {
            Analyze(frame.Message, framePointer);
        }

        private void Analyze(string message, string pointer)
        {
            if (DoesNotEndWithPeriod(message))
            {
                string messagePointer = pointer.AtProperty(SarifPropertyName.Message);

                LogResult(
                    messagePointer,
                    nameof(RuleResources.SV0008_Default),
                    message);
            }
        }

        private static bool DoesNotEndWithPeriod(string message)
        {
            return message != null && !message.EndsWith(".", StringComparison.Ordinal);
        }
    }
}
