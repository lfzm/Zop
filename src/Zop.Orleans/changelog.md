## Zop开发框架 Orleans服务核心抽象层

### [2.0 beta1]

-主要功能
	-应用服务抽象类
		-继承了Orleans Grain的基础类。
		-添加了内部访问Grain统一接口 IApplicationService<TState> ，通过扩展方法'GetStateGrain'可以快速查询领域实体的数据。
	-异常统一处理
		-为了避免Orleans异常类型化的问题，统一处理异常。
	-扩展方法
		-在Orleans KeyedServiceCollection基础上添加对DI系统的AddScoped支持。
	-仓储
		-实现Orleans IGrainStorage ，对关系型数据的存储进行支持。

### [2.0.0.10507]
	-BUG修复
		-仓储在Scoped模式下无法获取第二个实体的存储的问题。