﻿/*
 *    The contents of this file are subject to the Initial
 *    Developer's Public License Version 1.0 (the "License");
 *    you may not use this file except in compliance with the
 *    License. You may obtain a copy of the License at
 *    https://github.com/FirebirdSQL/NETProvider/blob/master/license.txt.
 *
 *    Software distributed under the License is distributed on
 *    an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either
 *    express or implied. See the License for the specific
 *    language governing rights and limitations under the License.
 *
 *    All Rights Reserved.
 */

//$Authors = Jiri Cincura (jiri@cincura.net)

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace FirebirdSql.EntityFrameworkCore.Firebird.Metadata.Conventions
{
	public class FbConventionSetBuilder : RelationalConventionSetBuilder
	{
		public FbConventionSetBuilder(RelationalConventionSetBuilderDependencies dependencies)
			: base(dependencies)
		{ }

		public override ConventionSet AddConventions(ConventionSet conventionSet)
		{
			base.AddConventions(conventionSet);
			conventionSet.ModelInitializedConventions.Add(new RelationalMaxIdentifierLengthConvention(31));
			return conventionSet;
		}

		public static ConventionSet Build()
		{
			var serviceProvider = new ServiceCollection()
				.AddEntityFrameworkFirebird()
				.AddDbContext<DbContext>(o => o.UseFirebird("database=localhost:_.fdb;user=sysdba;password=masterkey;charset=utf8"))
				.BuildServiceProvider();

			using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				using (var context = serviceScope.ServiceProvider.GetService<DbContext>())
				{
					return ConventionSet.CreateConventionSet(context);
				}
			}
		}
	}
}
