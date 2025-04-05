using StackExchange.Redis;
using VistaLOS.Application.Services.Options;
using IDatabase = StackExchange.Redis.IDatabase;
using IServer = StackExchange.Redis.IServer;

namespace VistaLOS.Application.Services.Impl;

public class CacheConnector
{
    public IServer Server { get; }
    public IDatabase RedisDatabase { get; }
    public int RedisDatabaseNumber { get; }

    public CacheConnector(IConnectionMultiplexer connectionMultiplexer,
                        RedisOptions redisOptions)
    {
        RedisDatabaseNumber = redisOptions.DatabaseNumber;
        RedisDatabase = connectionMultiplexer.GetDatabase(RedisDatabaseNumber);

        var connectionString = redisOptions.ConnectionString;
        var hostAndPort = connectionString.IndexOf(",") == -1 ?
            connectionString :
            connectionString.Substring(0, connectionString.IndexOf(","));
        Server = connectionMultiplexer.GetServer(hostAndPort);
    }
}
