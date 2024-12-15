self.assetsInclude = [];

self.externalAssets = [
    {
        "url": "/"
    },
    {
        url: "_framework/blazor.web.js"
    },
    {
        "url": "Bit.Bswup.Sample.styles.css"
    }
];

self.serverHandledUrls = [
    /\/api\//,
    /\/odata\//,
    /\/jobs\//,
    /\/core\//,
    /\/healthchecks-ui/,
    /\/healthz/,
    /\/swagger/,
    /\/signin-/,
    /\/.well-known/,
    /\/sitemap.xml/
];

self.defaultUrl = "/";
self.isPassive = true;
self.errorTolerance = 'lax';
self.caseInsensitiveUrl = true;


// on apps with Prerendering enabled, to have the best experience for the end user un-comment the following two lines.
// more info: https://bitplatform.dev/bswup/service-worker
 self.noPrerenderQuery = 'no-prerender=true';
 self.disablePassiveFirstBoot = true;

self.importScripts('_content/Bit.Bswup/bit-bswup.sw.js');