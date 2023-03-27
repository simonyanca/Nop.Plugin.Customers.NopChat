using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Customers.NopChat.Services;

namespace Nop.Plugin.Customers.NopChat.Data
{
    [NopMigration("2023/03/01 09:09:17", "Customers.NopChat base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        #region Methods

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
            Create.TableFor<NopChatMessage>();
        }

        #endregion
    }
}