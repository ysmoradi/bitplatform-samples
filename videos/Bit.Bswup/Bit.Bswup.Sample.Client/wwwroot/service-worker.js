self.assetsInclude = [];

self.externalAssets = [
    {
        "url": "/"
    },
    {
        url: "_framework/blazor.web.js"
    },
    {
        url: "Bit.Bswup.Sample.styles.css"
    },
    {
        url: "Bit.Bswup.Sample.Client.bundle.scp.css"
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

self.noPrerenderQuery = 'no-prerender=true';

// If you wish to disable pre-rendering in App.razor, comment out the following line as well.
self.disablePassiveFirstBoot = true;

self.importScripts('_content/Bit.Bswup/bit-bswup.sw.js');