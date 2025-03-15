/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./**/*.{html,js,razor}"
  ],
  theme: {
    colors: {
      "background" : "#131126",
      "white" : "#FFFFFF",
      "titlecolor": "#262626"
    },
    extend: {
      fontFamily: {
        sans: ["Open Sans", "sans-serif"],
        serif: ["Titillium Web", "sans-serif"],
      },
    },
  },
  plugins: [],
}
