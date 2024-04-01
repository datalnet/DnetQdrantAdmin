'use strict';

const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

var config = {
    optimization: {
    },

    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: "css-loader",
                        options: {
                            url: false
                        }
                    },
                    {
                        loader: "sass-loader",
                        options: {
                            sourceMap: true,
                            implementation: require('sass'),
                        },
                    },
                    {
                        loader: "postcss-loader",
                        options: {
                            postcssOptions: {
                                plugins: [
                                    [
                                        "autoprefixer",
                                    ],
                                ],
                            },
                        },
                    },
                ],
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: 'babel-loader',
                        options: {
                            presets: ['@babel/preset-env']
                        }
                    }
                ]
            }
        ]
    },

    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css",
        }),
    ]
};

var siteConfig = Object.assign({}, config, {
    entry: {
        app: path.resolve(__dirname, "BlazeOrbital.Client/assets/scss/app.scss"),
        main: path.resolve(__dirname, "BlazeOrbital.Client/assets/js/app.js"),
    },
    output: {
        path: path.resolve(__dirname, "BlazeOrbital/wwwroot/"),
    },
});

var workingConfig = (env) => {
    switch (env.appname) {
        case 'site':
            return siteConfig;
    }
};

module.exports = (env) => {
    return workingConfig(env);
};
