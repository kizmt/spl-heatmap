(function () {
    var margin = { top: 25, right: 25, bottom: 25, left: 25 },
        width = 1000,
        height = width / 2,
        formatNumber = d3.format(",d"),
        transitioning;



    var x = d3.scale.linear()
        .domain([0, width])
        .range([0, width]);

    var y = d3.scale.linear()
        .domain([0, height])
        .range([0, height]);

    var color = d3.scale.threshold()
        .domain([-3, -0.25, 0.25, 3])
        .range(["#FD499D", "#FD499D", "#064D15", "#0AD171", "#0AD171"]);

    var treemap = d3.layout.treemap()
        .children(function (d, depth) { return depth ? null : d._children; })
        .sort(function (a, b) { return a.value - b.value; })
        .ratio(height / width * 0.5 * (1 + Math.sqrt(5)))
        .round(false);

    var svg = d3.select("#heatmap").append("svg")
        .attr("width", width)
        .attr("height", height)
        .style("margin-left", -margin.left + "px")
        .style("margin.right", -margin.right + "px")
        .call(responsivefy) // Call responsivefy to make the chart responsive
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")")
        .style("shape-rendering", "crispEdges");

    var grandparent = svg.append("g")
        .attr("class", "grandparent");

    grandparent.append("rect")
        .attr("y", -margin.top)
        .attr("width", width)
        .attr("height", margin.top);

    grandparent.append("text")
        .attr("x", 6)
        .attr("y", 6 - margin.top)
        .attr("dy", ".75em");

    d3.queue()
        .defer(d3.json, "data.json")
        .await(function (error, root) {
            if (error) throw error;
            initialize(root);
            accumulate(root);
            layout(root);
            display(root);

            function initialize(root) {
                root.x = root.y = 0;
                root.dx = width;
                root.dy = height;
                root.depth = 0;
            }

            function accumulate(d) {
                return (d._children = d.children)
                    ? d.value = d.children.reduce(function (p, v) { return p + accumulate(v); }, 0)
                    : d.value;
            }

            function layout(d) {
                if (d._children) {
                    treemap.nodes({ _children: d._children });
                    d._children.forEach(function (c) {
                        c.x = d.x + c.x * d.dx;
                        c.y = d.y + c.y * d.dy;
                        c.dx *= d.dx;
                        c.dy *= d.dy;
                        c.parent = d;
                        layout(c);
                    });
                }
            }

            function getContrast50(hexcolor) {
                return (parseInt(hexcolor.replace('#', ''), 16) > 0xffffff / 3) ? 'black' : 'white';
            }

            function display(d) {
                var formatter = new Intl.NumberFormat('en-US', {
                    style: 'currency',
                    currency: 'USD',

                    // These options are needed to round to whole numbers if that's what you want.
                    //minimumFractionDigits: 0, // (this suffices for whole numbers, but will print 2500.10 as $2,500.1)
                    //maximumFractionDigits: 0, // (causes 2500.99 to be printed as $2,501)
                });

                grandparent
                    .datum(d.parent)
                    .on("click", transition)
                    .select("text")
                    .text(name(d));

                grandparent
                    .datum(d.parent)
                    .select("rect")
                    .attr("fill", function () { return color(d['rate']) })

                var g1 = svg.insert("g", ".grandparent")
                    .datum(d)
                    .attr("class", "depth");

                var g = g1.selectAll("g")
                    .data(d._children)
                    .enter().append("g");

                g.filter(function (d) { return d._children; })
                    .classed("children", true)
                    .on("click", transition);

                g.selectAll(".child")
                    .data(function (d) { return d._children || [d]; })
                    .enter().append("rect")
                    .attr("class", "child")
                    .call(rect);

                d3.select("#heatmap").select("#tooltip").remove();
                var div = d3.select("#heatmap").append("div")
                    .attr("id", "tooltip")
                    .style("opacity", 0);




                g.append("svg:a")
                    .attr("xlink:href", function (d) {
                        return d.url;
                    })
                    .append("rect")
                    .attr("class", "parent")
                    .call(rect)
                    .on("mouseover", function (d) {
                        if (d.parent.name != "MARKET") {
                            d3.select("#tooltip").transition()
                                .duration(200)
                                .style("opacity", 1);
                            d3.select("#tooltip").html("<h3>" + d.name + " (" + d.tickersymbol + ")</h3><table>" +
                                "<tr><td><img src='" + d.image + "' style='width:15px;height:15px;'/></td><td>market cap: " + formatter.format(d.value) + "</td></tr><tr><td></td><td>price: " + formatter.format(d.currentprice) + "</td></tr><tr><td></td><td>24h change " + d.rate + "%</td></tr>" +
                                "</table>")
                                .style("left", (d3.event.pageX - document.getElementById('heatmap').offsetLeft + 20) + "px")
                                .style("top", (d3.event.pageY - document.getElementById('heatmap').offsetTop - 60) + "px");
                        }
                    })
                    .on("mouseout", function (d) {
                        d3.select("#tooltip").transition()
                            .duration(500)
                            .style("opacity", 0);
                    })
                    .append("title")
                    .text(function (d) { return formatNumber(d.value); });


                g.append("text")
                    .attr("dy", ".75em")
                    .text(function (d) { return d.name; })
                    .call(text);

                g.append("image")
                    .attr('width', 20)
                    .attr('height', 20)
                    .call(seticon);

                //g.append("image")
                //    .attr('x', 5)
                //    .attr('y', 5)
                //    .attr('width', 20)
                //    .attr('height', 20)
                //    .attr("xlink:href", function (d) {
                //        return d.image;
                //    });

                function transition(d) {
                    if (transitioning || !d) return;
                    transitioning = true;

                    var g2 = display(d),
                        t1 = g1.transition().duration(750),
                        t2 = g2.transition().duration(750);

                    x.domain([d.x, d.x + d.dx]);
                    y.domain([d.y, d.y + d.dy]);

                    svg.style("shape-rendering", null);

                    svg.selectAll(".depth").sort(function (a, b) { return a.depth - b.depth; });

                    g2.selectAll("text").style("fill-opacity", 0);

                    t1.selectAll("text").call(text).style("fill-opacity", 0);
                    t2.selectAll("text").call(text).style("fill-opacity", 1);
                    t1.selectAll("rect").call(rect);
                    t2.selectAll("rect").call(rect);

                    t1.remove().each("end", function () {
                        svg.style("shape-rendering", "crispEdges");
                        transitioning = false;
                    });
                }

                return g;
            }

            function seticon(icon) {
                icon.attr("x", function (d) { return x(d.x) + (x(d.x + d.dx) - x(d.x)) / 2.2; })
                    .attr("y", function (d) { return y(d.y) + (y(d.y + d.dy) - y(d.y)) / 1.8; })
                    .attr("width", function (d) { return (x(d.x + d.dx) - x(d.x))/10; })
                    .attr("height", function (d) { return y((d.y + d.dy) - y(d.y))/10; })
                .attr("xlink:href", function (d) {
                    return d.image;
                });
            }

            function text(text) {
                text.attr("x", function (d) { return x(d.x) + (x(d.x + d.dx) - x(d.x)) / 2; })
                    .attr("y", function (d) { return y(d.y) + (y(d.y + d.dy) - y(d.y)) / 2; })
                    .attr("dy", 0)
                    .attr("font-size", function (d) {
                        var w = x(d.x + d.dx) - x(d.x),
                            h = y(d.y + d.dy) - y(d.y),
                            t = (d.name).length / 1.3;
                        var tf = Math.min(Math.floor(w / t), h / 3);
                        return (tf >= 5) ? Math.min(tf, 30) : 0;
                    })
                    .attr("fill", "white")
                    .attr("text-anchor", "middle");
            }

            function rect(rect) {
                rect.attr("x", function (d) { return x(d.x); })
                    .attr("y", function (d) { return y(d.y); })
                    .attr("width", function (d) { return x(d.x + d.dx) - x(d.x); })
                    .attr("height", function (d) { return y(d.y + d.dy) - y(d.y); })
                    .attr("fill", function (d) { return color(parseFloat(d.rate)); });
            }

            function name(d) {
                return "SPL token heatmap";
            }

        });
    function responsivefy(svg) {

        // Container is the DOM element, svg is appended.
        // Then we measure the container and find its
        // aspect ratio.
        const container = d3.select(svg.node().parentNode),
            width = parseInt(svg.style('width'), 10),
            height = parseInt(svg.style('height'), 10),
            aspect = width / height;

        // Add viewBox attribute to set the value to initial size
        // add preserveAspectRatio attribute to specify how to scale
        // and call resize so that svg resizes on page load
        svg.attr('viewBox', `0 0 ${width} ${height}`).
            attr('preserveAspectRatio', 'xMinYMid').
            call(resize);

        d3.select(window).on('resize.' + container.attr('id'), resize);

        function resize() {
            const targetWidth = parseInt(container.style('width'));
            svg.attr('width', targetWidth);
            svg.attr('height', Math.round(targetWidth / aspect));
        }
    }
}());