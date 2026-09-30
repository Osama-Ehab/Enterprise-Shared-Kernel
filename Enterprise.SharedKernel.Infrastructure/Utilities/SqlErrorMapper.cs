using Enterprise.SharedKernel.Enums;
using Enterprise.SharedKernel.Interfaces.Repositories;
using Enterprise.SharedKernel.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Enterprise.SharedKernel.Infrastructure.Utilities
{
    public interface ISqlErrorMapper
    {
        ErrorType GetErrorType(int sqlNumber);
        string GetMessage(SqlException ex);
    }

    public class SqlErrorMapper : ISqlErrorMapper
    {
        // Map ID -> Config Object (Not just a string anymore)
        // الذاكرة (RAM) التي ستحمل الأخطاء
        private readonly Dictionary<int, ErrorConfig> _errorConfigs;

        // سيقوم النظام تلقائياً بتمرير البيانات هنا عند الإقلاع بفضل IOptions
        public SqlErrorMapper(IOptions<Dictionary<int, ErrorConfig>> options)
        {
            _errorConfigs = options.Value;
        }

        // 1. Get Type (Simple Lookup)
        public ErrorType GetErrorType(int sqlNumber)
        {
            if (_errorConfigs.TryGetValue(sqlNumber, out var config))
            {
                return Enum.TryParse(config.Type, true, out ErrorType type) ? type : ErrorType.Database;
            }
            return ErrorType.Database;
        }

        // 2. Get Message (Smart Contextual Lookup)
        public string GetMessage(SqlException ex)
        {
            if (_errorConfigs.TryGetValue(ex.Number, out var config))
            {
                // A. Check specific contexts defined in JSON
                if (config.Contexts != null && config.Contexts.Any())
                {
                    // Find the first context where the SQL message contains the keyword
                    var match = config.Contexts.FirstOrDefault(ctx => ex.Message.Contains(ctx.Contains));

                    if (match != null)
                    {
                        return match.Message;
                    }
                }

                // B. If no context matches, return default
                return config.DefaultMessage;
            }

            return "An undefined database error occurred.";
        }
    }
}
