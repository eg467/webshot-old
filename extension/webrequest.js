

    chrome.webRequest.onAuthRequired.addListener(
    function handler(details){
     return {'authCredentials': {username: "eric", password: "ericpass123123"}};
    },
    {urls:["<all_urls>"]},
    ['blocking']);

