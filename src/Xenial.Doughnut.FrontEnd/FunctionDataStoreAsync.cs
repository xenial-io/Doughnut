using System;
using System.Threading;
using System.Threading.Tasks;

using BIT.Data.Functions;
using BIT.Data.Services;
using BIT.Xpo.Models;

using DevExpress.Xpo.DB;
using DevExpress.Xpo.Helpers;

namespace Xenial.Doughnut.FrontEnd
{
    public abstract class FunctionDataStoreAsync : IDataStore, IDataStoreAsync, ICommandChannel
    {
        IFunction FunctionClient { get; set; }
        IFunctionAsync FunctionClientAsync { get; set; }
        IObjectSerializationService objectSerializationHelper;
        public FunctionDataStoreAsync(IFunction functionClient, IObjectSerializationService objectSerializationHelper, AutoCreateOption autoCreateOption)
        {
            this.FunctionClient = functionClient;
            if (functionClient is IFunctionAsync functionAsync)
            {
                this.FunctionClientAsync = functionAsync;
            }

            this.objectSerializationHelper = objectSerializationHelper;
        }

        public AutoCreateOption AutoCreateOption { get; }

        public virtual ModificationResult ModifyData(params ModificationStatement[] dmlStatements)
        {
            IDataParameters Parameters = new DataParameters();
            Parameters.MemberName = nameof(ModifyData);
            Parameters.ParametersValue = this.objectSerializationHelper.ToByteArray<ModificationStatement[]>(dmlStatements);
            var DataResult = FunctionClient.ExecuteFunction(Parameters);
            var ModificationResults = this.objectSerializationHelper.GetObjectsFromByteArray<ModificationResult>(DataResult.ResultValue);
            return ModificationResults;
        }
        public async virtual Task<ModificationResult> ModifyDataAsync(CancellationToken cancellationToken, params ModificationStatement[] dmlStatements)
        {
            IDataParameters Parameters = new DataParameters();
            Parameters.MemberName = nameof(ModifyData);
            Parameters.ParametersValue = this.objectSerializationHelper.ToByteArray<ModificationStatement[]>(dmlStatements);
            var DataResult = await FunctionClientAsync.ExecuteFunctionAsync(Parameters);
            var ModificationResults = this.objectSerializationHelper.GetObjectsFromByteArray<ModificationResult>(DataResult.ResultValue);
            return ModificationResults;
        }

        public virtual SelectedData SelectData(params SelectStatement[] selects)
        {
            IDataParameters Parameters = new DataParameters();
            Parameters.MemberName = nameof(SelectData);
            Parameters.ParametersValue = this.objectSerializationHelper.ToByteArray<SelectStatement[]>(selects);
            var DataResult = FunctionClient.ExecuteFunction(Parameters);
            var SelectedData = this.objectSerializationHelper.GetObjectsFromByteArray<SelectedData>(DataResult.ResultValue);
            return SelectedData;
        }

        public async virtual Task<SelectedData> SelectDataAsync(CancellationToken cancellationToken, params SelectStatement[] selects)
        {
            IDataParameters Parameters = new DataParameters();
            Parameters.MemberName = nameof(SelectData);
            Parameters.ParametersValue = this.objectSerializationHelper.ToByteArray<SelectStatement[]>(selects);
            var DataResult = await FunctionClientAsync.ExecuteFunctionAsync(Parameters);
            var SelectedData = this.objectSerializationHelper.GetObjectsFromByteArray<SelectedData>(DataResult.ResultValue);
            return SelectedData;
        }

        public virtual UpdateSchemaResult UpdateSchema(bool dontCreateIfFirstTableNotExist, params DBTable[] tables)
        {
            IDataParameters Parameters = new DataParameters();
            UpdateSchemaParameters updateSchemaParameters = new UpdateSchemaParameters(dontCreateIfFirstTableNotExist, tables);
            Parameters.MemberName = nameof(UpdateSchema);
            Parameters.ParametersValue = this.objectSerializationHelper.ToByteArray<UpdateSchemaParameters>(updateSchemaParameters);
            IDataResult DataResult = FunctionClient.ExecuteFunction(Parameters);
            var UpdateSchemaResult = this.objectSerializationHelper.GetObjectsFromByteArray<UpdateSchemaResult>(DataResult.ResultValue);
            return UpdateSchemaResult;
        }

        public async virtual Task<UpdateSchemaResult> UpdateSchemaAsync(CancellationToken cancellationToken, bool doNotCreateIfFirstTableNotExist, params DBTable[] tables)
        {
            IDataParameters Parameters = new DataParameters();
            UpdateSchemaParameters updateSchemaParameters = new UpdateSchemaParameters(doNotCreateIfFirstTableNotExist, tables);
            Parameters.MemberName = nameof(UpdateSchema);
            Parameters.ParametersValue = this.objectSerializationHelper.ToByteArray<UpdateSchemaParameters>(updateSchemaParameters);
            IDataResult DataResult = await FunctionClientAsync.ExecuteFunctionAsync(Parameters);
            var UpdateSchemaResult = this.objectSerializationHelper.GetObjectsFromByteArray<UpdateSchemaResult>(DataResult.ResultValue);
            return UpdateSchemaResult;
        }

        protected virtual object Do(string command, object args)
        {
            IDataParameters Parameters = new DataParameters();
            CommandChannelDoParams commandChannelDoParams = new CommandChannelDoParams(command, args);
            Parameters.MemberName = nameof(Do);
            Parameters.ParametersValue = this.objectSerializationHelper.ToByteArray<CommandChannelDoParams>(commandChannelDoParams);
            IDataResult DataResult = FunctionClient.ExecuteFunction(Parameters);


            switch (commandChannelDoParams.Command)
            {

                case CommandChannelHelper.Command_ExecuteScalarSQL:
                case CommandChannelHelper.Command_ExecuteScalarSQLWithParams:
                    return this.objectSerializationHelper.GetObjectsFromByteArray<object>(DataResult.ResultValue);


                case CommandChannelHelper.Command_ExecuteQuerySQL:
                case CommandChannelHelper.Command_ExecuteQuerySQLWithParams:
                case CommandChannelHelper.Command_ExecuteQuerySQLWithMetadata:
                case CommandChannelHelper.Command_ExecuteQuerySQLWithMetadataWithParams:
                case CommandChannelHelper.Command_ExecuteStoredProcedure:
                case CommandChannelHelper.Command_ExecuteStoredProcedureParametrized:
                    return this.objectSerializationHelper.GetObjectsFromByteArray<SelectedData>(DataResult.ResultValue);


                case CommandChannelHelper.Command_ExecuteNonQuerySQL:
                case CommandChannelHelper.Command_ExecuteNonQuerySQLWithParams:
                    return this.objectSerializationHelper.GetObjectsFromByteArray<int>(DataResult.ResultValue);


                default:
                    throw new Exception($"ICommandChannel Do method retuned an unknow data type while processing {commandChannelDoParams.Command}");
            }
        }

        object ICommandChannel.Do(string command, object args)
        {
            return this.Do(command, args);
        }
    }
}
