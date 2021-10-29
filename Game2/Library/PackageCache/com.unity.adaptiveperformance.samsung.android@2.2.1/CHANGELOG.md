# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [2.2.1] - 2021-05-05

### Changed
- Scalers do not use switch-case to determine their value, instead use min, max scale and max level to calculate a scale and increment and apply it to their target when the level changes. VRR scaler specifically.

### Added
- Boost mode
- Cluster info

## [2.1.1] - 2021-02-03

### Changed
- Fix FB: 1305455 IndexOutOfRangeException error thrown on changing VRR Refresh Rate on device.
- Fix FB: 1309052 IndexOutOfRangeException error thrown on changing VRR Refresh Rate on Device Simulator.

## [2.1.0] - 2020-10-12

### Changed
- Updated the version defines for the device simulator to support it in 2021.1 without package.
- Updated the version to keep in sync with the main Adaptive Performance package.

## [2.0.2] - 2020-08-21

### Changed
- Improved Stats reporting.

### Removed
- Folders and files which are not needed by Adaptive Performance from the package.

## [2.0.1] - 2020-08-10

### Changed
- Updated GameSDK wrapper to latest version which enhances GPU frametime information

## [2.0.0] - 2020-06-05

### Added
- Variable Refresh Rate API (VRR) to support multiple vsync displays
- Verified support for 2020.2 and minimum support 2019 LTS+. Please use Adaptive Performance Samsung Android 1.x for earlier versions supporting 2018.3+.
