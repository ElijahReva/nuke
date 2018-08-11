﻿// Copyright 2018 Maintainers and Contributors of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.Execution;

namespace Nuke.Common.ProjectModel
{
    /// <inheritdoc />
    [PublicAPI]
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public class SolutionAttribute : StaticInjectionAttributeBase
    {
        private readonly string _solutionFileRootRelativePath;

        [Obsolete("With the next release " + NukeBuild.ConfigurationFile + " will be removed. " +
                  "Please specify the path to the solution as single argument to the " + nameof(SolutionAttribute) + " instead.")]
        public SolutionAttribute()
            :this(solutionFileRootRelativePath: null)
        {
        }

        public SolutionAttribute(string solutionFileRootRelativePath)
        {
            _solutionFileRootRelativePath = solutionFileRootRelativePath;
        }
        
        [CanBeNull]
        public static string Configuration { get; set; }

        [CanBeNull]
        public static string TargetFramework { get; set; }

        public static Solution Value { get; private set; }

        [CanBeNull]
        public override object GetStaticValue()
        {
            var solutionFile = _solutionFileRootRelativePath != null
                ? Path.Combine(NukeBuild.Instance.RootDirectory, _solutionFileRootRelativePath)
                : NukeBuild.Instance.SolutionFile;
            
            return Value = Value ??
                           ProjectModelTasks.ParseSolution(
                               solutionFile,
                               Configuration ?? NukeBuild.Instance.Configuration,
                               TargetFramework);
        }
    }
}
