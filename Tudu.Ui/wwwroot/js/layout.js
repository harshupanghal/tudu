window.layout = (function () {
    const mq = window.matchMedia('(min-width: 1024px)');
    let dotnetRef = null;

    function isDesktop() {
        return mq.matches;
    }

    function applyScrollLock(locked) {
        // Lock scroll when overlay drawer is open
        document.documentElement.classList.toggle('no-scroll', locked);
        document.body.classList.toggle('no-scroll', locked);
    }

    function subscribeBreakpoint(dotnetObjRef) {
        dotnetRef = dotnetObjRef;
        mq.addEventListener('change', handleChange);
    }

    function handleChange(e) {
        if (dotnetRef) {
            dotnetRef.invokeMethodAsync('OnBreakpointChanged', e.matches);
        }
    }

    return { isDesktop, applyScrollLock, subscribeBreakpoint };
})();
