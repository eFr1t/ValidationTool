﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ODataValidator.Rule.Feed
{
    #region Namespace.
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using ODataValidator.RuleEngine;
    using ODataValidator.RuleEngine.Common;
    #endregion

    /// <summary>
    /// Class of extension rule for Feed.Core.4620
    /// </summary>
    [Export(typeof(ExtensionRule))]
    public class FeedCore4620 : ExtensionRule
    {
        /// <summary>
        /// Gets Category property
        /// </summary>
        public override string Category
        {
            get
            {
                return "core";
            }
        }

        /// <summary>
        /// Gets rule name
        /// </summary>
        public override string Name
        {
            get
            {
                return "Feed.Core.4620";
            }
        }

        /// <summary>
        /// Gets rule description
        /// </summary>
        public override string Description
        {
            get
            {
                return @"If the entity set cannot be determined from the context URL of the feed, the atom:entry element MUST have a metadata:context attribute.";
            }
        }

        /// <summary>
        /// Gets rule specification in OData document
        /// </summary>
        public override string V4SpecificationSection
        {
            get
            {
                return "6.1.2";
            }
        }

        /// <summary>
        /// Gets location of help information of the rule
        /// </summary>
        public override string HelpLink
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the error message for validation failure
        /// </summary>
        public override string ErrorMessage
        {
            get
            {
                return this.Description;
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public override ODataVersion? Version
        {
            get
            {
                return ODataVersion.V4;
            }
        }

        /// <summary>
        /// Gets the requirement level.
        /// </summary>
        public override RequirementLevel RequirementLevel
        {
            get
            {
                return RequirementLevel.Must;
            }
        }

        /// <summary>
        /// Gets the payload type to which the rule applies.
        /// </summary>
        public override PayloadType? PayloadType
        {
            get
            {
                return RuleEngine.PayloadType.Feed;
            }
        }

        /// <summary>
        /// Gets the payload format to which the rule applies.
        /// </summary>
        public override PayloadFormat? PayloadFormat
        {
            get
            {
                return RuleEngine.PayloadFormat.Atom;
            }
        }

        /// <summary>
        /// Gets the IsOfflineContext property to which the rule applies.
        /// </summary>
        public override bool? IsOfflineContext
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the flag whether the rule requires metadata document
        /// </summary>
        public override bool? RequireMetadata
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Verify Feed.Core.4620
        /// </summary>
        /// <param name="context">Service context</param>
        /// <param name="info">out parameter to return violation information when rule fail</param>
        /// <returns>true if rule passes; false otherwise</returns>
        public override bool? Verify(ServiceContext context, out ExtensionRuleViolationInfo info)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            bool? passed = null;
            info = null;

            var payload = XElement.Parse(context.ResponsePayload);

            if (null != payload)
            {
                string xPath = "./*[local-name()='entry']";
                var entryElems = payload.XPathSelectElements(xPath, ODataNamespaceManager.Instance);

                if (null != entryElems && entryElems.Any())
                {
                    foreach (var elem in entryElems)
                    {
                        if (null != elem.GetAttributeValue("metadata:context", ODataNamespaceManager.Instance))
                        {
                            if (payload.GetAttributeValue("metadata:context", ODataNamespaceManager.Instance) != elem.GetAttributeValue("metadata:context", ODataNamespaceManager.Instance))
                            {
                                passed = true;
                            }
                            else
                            {
                                passed = false;
                                info = new ExtensionRuleViolationInfo(this.ErrorMessage, context.Destination, context.ResponsePayload);
                                break;
                            }
                        }
                    }
                }
            }

            return passed;
        }
    }
}
