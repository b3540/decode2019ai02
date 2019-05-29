// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EmptyBot v4.3.0

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using System.Threading.Tasks;

namespace MyBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Bot Framework �A�_�v�^�[�Ŏg�p���� ConfigurationCredentialProvider �쐬.
            services.AddSingleton<ICredentialProvider, ConfigurationCredentialProvider>();
            // �X�e�[�g�Ǘ�
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<UserState>();
            services.AddSingleton<ConversationState>();
            // Bot Framework �A�_�v�^�[.
            services.AddSingleton<IBotFrameworkHttpAdapter, BotFrameworkHttpAdapter>();
            // �R���g���[���[�ŗ��p���� IBot �o�^
            services.AddTransient<IBot, GraphBot>();

            // IRecognizer �Ƃ��� LuisRecognizer ��o�^ 
            var luisApplication = new LuisApplication(
                Configuration["LuisAppId"],
                Configuration["LuisAPIKey"],
                "https://" + Configuration["LuisAPIHostName"]
            );
            var recognizer = new LuisRecognizer(luisApplication);
            services.AddSingleton<IRecognizer>(recognizer);

            // MSGraph �N���C�A���g�o�^
            services.AddTransient<IGraphServiceClient>(sp => new GraphServiceClient(new DelegateAuthenticationProvider((request) => { return Task.CompletedTask; })));
            services.AddTransient(sp => new MSGraphService(sp.GetRequiredService<IGraphServiceClient>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
