import { extendTheme, ThemeConfig } from "@chakra-ui/react";

const config: ThemeConfig = {
  initialColorMode: "dark",
  useSystemColorMode: false,
};

const colors = {
  // Base colors
  primary: {
    50: "#e3f2ff",
    100: "#b3daff",
    200: "#80c0ff",
    300: "#4da6ff",
    400: "#1a8cff",
    500: "#0077e6", // main primary
    600: "#0062b4",
    700: "#004f8c",
    800: "#003b64",
    900: "#00273f",
  },
  secondary: {
    50: "#fff0e6",
    100: "#ffd1b3",
    200: "#ffb380",
    300: "#ff944d",
    400: "#ff751a",
    500: "#e65c00",
    600: "#b44900",
    700: "#803300",
    800: "#4d1f00",
    900: "#1f0a00",
  },
  gray: {
    50: "#f8f9fa",
    100: "#e9ecef",
    200: "#dee2e6",
    300: "#ced4da",
    400: "#adb5bd",
    500: "#6c757d",
    600: "#495057",
    700: "#343a40",
    800: "#212529",
    900: "#121314",
  },
  background: {
    light: "#f8f9fa",
    dark: "#121314", // main dark background
  },
  success: "#38a169",
  warning: "#dd6b20",
  danger: "#e53e3e",
};

const theme = extendTheme({
  config,
  colors,
  fonts: {
    heading: "'Inter', sans-serif",
    body: "'Inter', sans-serif",
  },
  styles: {
    global: {
      body: {
        bg: colors.background.dark,
        color: "gray.100",
      },
      a: {
        _hover: {
          textDecoration: "none",
        },
      },
    },
  },
  components: {
    Button: {
      baseStyle: {
        borderRadius: "md",
      },
      variants: {
        solid: {
          bg: colors.primary[500],
          color: "white",
          _hover: {
            bg: colors.primary[600],
          },
        },
        outline: {
          borderColor: colors.primary[500],
          color: colors.primary[500],
          _hover: {
            bg: colors.primary[600],
            color: "white",
          },
        },
      },
    },
    Input: {
      variants: {
        filled: {
          field: {
            // bg: "gray.800",
            _hover: {
              // bg: "gray.700",
            },
            _focus: {
              // bg: "gray.700",
              borderColor: colors.primary[500],
            },
          },
        },
      },
    },
  },
});

export default theme;
