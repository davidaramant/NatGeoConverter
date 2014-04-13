function toggleHorizontal() {
    var pageScan = document.getElementById('pageScan');
    if(pageScan && pageScan.style) {
        pageScan.style.height = 'auto';
        pageScan.style.width = '100%';
    }
}

function toggleVertical() {
    var pageScan = document.getElementById('pageScan');
    if(pageScan && pageScan.style) {
        var w = window,
        d = document,
        e = d.documentElement,
        g = d.getElementsByTagName('body')[0],
        x = w.innerWidth || e.clientWidth || g.clientWidth,
        y = w.innerHeight|| e.clientHeight|| g.clientHeight;
        
        pageScan.style.height = (y - 60).toString() + 'px';
        pageScan.style.width = 'auto';
    }
}