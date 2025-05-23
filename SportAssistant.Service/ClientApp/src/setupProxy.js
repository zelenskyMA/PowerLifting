const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:56144';

const context =  [
  "/trainingPlan", "/planDay", "/planExercise", "/exerciseInfo", "/analitics",
  "/templateSet", "/templatePlan", "/templateDay", "/templateExercise",

  "/user", "/userInfo", "/userAchivement",

  "/administration", "/dictionary", "/appSettings", "/reports",
  "/organization", "/manager", "/assignedCoach",

  "/trainingRequests", "/trainingGroups", "/groupUser"
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};
