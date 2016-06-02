﻿//------------------------------------------------------------------------------
// <copyright file="VSPackageGuids.cs" company="Company">
//         Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Excess.VS
{
    using System;

    /// <summary>
    /// VSPackage GUID constants.
    /// </summary>
    internal static class VSPackageGuids
    {
        /// <summary>
        /// VSPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "d3c4166c-b9e0-48f3-a01d-c46d38c4ed2e";

        public const string DesignerEditorFactoryString = "6bf3ea12-98bb-41e2-ba01-8662f713d293";
        public static readonly Guid DesignerEditorFactory = new Guid(DesignerEditorFactoryString);

    }
}